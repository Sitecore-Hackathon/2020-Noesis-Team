using Sitecore.Data;

namespace Noesis.Feature.MarketModule
{
    public struct Templates
    {
        public struct ModulePage
        {
            public static readonly ID ID = new ID("{4EE13B07-C46A-4DC8-8AF5-AA92EAE9A187}");

            public struct Fields
            {
                public static readonly ID Category = new ID("{C95C1801-8A3C-43B1-991F-F8523F267136}");
                public static readonly ID Requirements = new ID("{AAC6B0BD-E9E0-4018-ABEC-892AE5C19862}");
                public static readonly ID Rating = new ID("{7CC42499-755C-48FD-AA27-DCAE678B8954}");
                public static readonly ID Downloads = new ID("{6E8B68CF-15B4-4276-A82C-0F3CF6F0E588}");
            }
        }

        public struct PageData
        {
            public static readonly ID ID = new ID("{1C82E550-EBCD-4E5D-8ABD-D50D0809541E}");
        }

        public struct Text
        {
            public static readonly ID ID = new ID("{B8F5894C-B493-4016-BF78-4090B0A2FD16}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{B3635965-6786-457C-9DEE-28DE73C0A331}");
            }
        }

        public struct Link
        {
            public static readonly ID ID = new ID("{E24E0789-A058-48BF-ABB6-C69C0E60B6B7}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{DD05144A-BD66-4EA3-956A-4B7104DAABA3}");
            }
        }

        public struct Gallery
        {
            public static readonly ID ID = new ID("{57BCF1CE-35CB-4752-B23B-2D4522AF6292}");
        }

        public struct GalleryImage
        {
            public static readonly ID ID = new ID("{9D77C212-A828-4792-8E72-749F19C0154D}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{CBDAAB5F-3F7A-4072-8B7F-6A325A4A2205}");
            }
        }

        public struct Content
        {
            public static readonly ID ModulesFolder = new ID("{CF47808E-9CC5-4000-848A-7A827F75CE54}");
        }
    }
}