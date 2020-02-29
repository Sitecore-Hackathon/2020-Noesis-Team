using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Sitecore.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Noesis.Feature.MarketModule.Jobs
{
    public class MarketModuleImporter
    {
        private static readonly Database MasterDb = Sitecore.Configuration.Factory.GetDatabase("master");

        public void Process(Item[] items, CommandItem command, ScheduleItem schedule)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["MarketModule"].ConnectionString;

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var comm = new SqlCommand("Select * from Module", conn))
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = ItemUtil.ProposeValidItemName(reader["Name"].ToString());
                        var description = reader["Description"].ToString();
                        var overview = reader["Overview"].ToString();
                        var category = reader["Category"].ToString();
                        var requirements = reader["Requirements"].ToString();
                        var file = reader["ModuleFile"] as byte[];
                        var image1 = reader["Image1"] as byte[];
                        var image2 = reader["Image2"] as byte[];
                        var image3 = reader["Image3"] as byte[];

                        CreateItem(name, description, overview, category, requirements, file, image1, image2, image3);
                    }
                }
            }
        }

        private void CreateItem(string name, string description, string overview, string category, string requirements, byte[] file, byte[] image1, byte[] image2, byte[] image3)
        {
            using (new SecurityDisabler())
            {
                var root = MasterDb.GetItem(Templates.Content.ModulesFolder);
                if (root == null) return;

                var pageAlreadyExists = root.GetChildren().Any(x => x.TemplateID == Templates.ModulePage.ID && x.Name == name);
                if (pageAlreadyExists) return;

                var modulePageItem = root.Add(name, new TemplateID(Templates.ModulePage.ID));
                if (modulePageItem == null) return;

                modulePageItem.Editing.BeginEdit();
                modulePageItem.Fields[Templates.ModulePage.Fields.Category].Value = category;
                modulePageItem.Fields[Templates.ModulePage.Fields.Requirements].Value = requirements;
                modulePageItem.Fields[Templates.ModulePage.Fields.Rating].Value = "0";
                modulePageItem.Fields[Templates.ModulePage.Fields.Downloads].Value = "0";
                modulePageItem.Editing.EndEdit();

                var pageDataItem = modulePageItem.Add("Data", new TemplateID(Templates.PageData.ID));

                var descriptionItem = pageDataItem?.Add("Description", new TemplateID(Templates.Text.ID));
                if (descriptionItem == null) return;

                descriptionItem.Editing.BeginEdit();
                descriptionItem.Fields[Templates.Text.Fields.Text].Value = description;
                descriptionItem.Editing.EndEdit();

                var overviewItem = pageDataItem.Add("Overview", new TemplateID(Templates.Text.ID));
                if (overviewItem == null) return;

                overviewItem.Editing.BeginEdit();
                overviewItem.Fields[Templates.Text.Fields.Text].Value = overview;
                overviewItem.Editing.EndEdit();

                var galleryItem = pageDataItem.Add("Gallery", new TemplateID(Templates.Gallery.ID));

                var images = new[] {image1, image2, image3};
                var count = 0;

                foreach (var image in images)
                {
                    if (image != null)
                    {
                        count++;
                        var galleryImageItem = galleryItem?.Add($"Gallery Image {count}", new TemplateID(Templates.GalleryImage.ID));
                        if (galleryImageItem == null) continue;

                        var imageName = $"{name}-{count}";
                        var imageItem = AddMediaItem(image, $"/sitecore/media library/Images/Modules/{imageName}", $"{imageName}.png");

                        if (imageItem != null)
                        {
                            galleryImageItem.Editing.BeginEdit();
                            ImageField imageField = galleryImageItem.Fields[Templates.GalleryImage.Fields.Image];
                            imageField.MediaID = imageItem.ID;
                            imageField.LinkType = "media";
                            galleryImageItem.Editing.EndEdit();
                        }
                    }
                }

                var downloadItem = pageDataItem.Add("Download", new TemplateID(Templates.Link.ID));
                if (downloadItem == null) return;

                if (file != null)
                {
                    var fileItem = AddMediaItem(file, $"/sitecore/media library/Files/Modules/{name}", $"{name}.zip");

                    downloadItem.Editing.BeginEdit();
                    LinkField linkField = downloadItem.Fields[Templates.Link.Fields.Link];
                    linkField.Text = "Download";
                    linkField.LinkType = "media";
                    linkField.TargetID = fileItem.ID;
                    downloadItem.Editing.EndEdit();
                }
            }
        }

        private static Item AddMediaItem(byte[] fileBuffer, string fullMediaPath, string fileNameWithExtension)
        {
            try
            {
                var db = Sitecore.Configuration.Factory.GetDatabase("master");

                var options = new MediaCreatorOptions
                {
                    FileBased = false,
                    IncludeExtensionInItemName = false,
                    Versioned = false,
                    Destination = fullMediaPath,
                    Database = db
                };

                var creator = new MediaCreator();
                var fileStream = new MemoryStream(fileBuffer);

                return creator.CreateFromStream(fileStream, fileNameWithExtension, options);
            }
            catch
            {
                return null;
            }
        }
    }
}