using Noesis.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using System.Data.SqlClient;
using System.Linq;

namespace Noesis.Feature.MarketModule.SubmitActions
{
    public class PopulateModule : SubmitActionBase<string>
    {
        public PopulateModule(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            const string cmdString = "INSERT INTO Module (Name,Category,Requirements,Description,Overview,Contributor,ModuleFile,Image1,Image2,Image3) VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8,@val9,@val10)";

            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["MarketModule"].ConnectionString;

            var moduleName = FormExtensions.GetValue(formSubmitContext, "ModuleName");
            var category = FormExtensions.GetValue(formSubmitContext, "CategoryList");
            var requirements = FormExtensions.GetValue(formSubmitContext, "RequirementsList");
            var description = FormExtensions.GetValue(formSubmitContext, "Description");
            var overview = FormExtensions.GetValue(formSubmitContext, "Overview");
            var file = FormExtensions.GetFileValue(formSubmitContext, "FileUpload");
            var image = FormExtensions.GetFileValue(formSubmitContext, "ImageUpload");

            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand())
            {
                comm.Connection = conn;
                comm.CommandText = cmdString;
                comm.Parameters.AddWithValue("@val1", moduleName);
                comm.Parameters.AddWithValue("@val2", category);
                comm.Parameters.AddWithValue("@val3", requirements);
                comm.Parameters.AddWithValue("@val4", description);
                comm.Parameters.AddWithValue("@val5", overview);
                comm.Parameters.AddWithValue("@val6", Sitecore.Context.GetUserName());
                comm.Parameters.AddWithValue("@val7", file.FirstOrDefault());
                
                for (var i = 0; i < 3; i++)
                {
                    var img = image.Count >= i ? image[i] : null;

                    comm.Parameters.AddWithValue($"@val{i+8}", img);
                }
                
                conn.Open();
                comm.ExecuteNonQuery();
            }
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            return true;
        }
    }
}