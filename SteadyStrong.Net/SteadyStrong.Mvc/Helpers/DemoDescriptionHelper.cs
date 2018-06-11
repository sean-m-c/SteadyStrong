namespace SteadyStrong.Mvc.Helpers
{
    public static class DemoDescriptionHelper
    {
        public static string GetPageIdFromUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(url)) return string.Empty;

            string descriptionId = "";
            string[] segments = url.Split("/");
            int i = 0;
            while (i < 3)
            {
                if (i < segments.Length && !string.IsNullOrWhiteSpace(segments[i]))
                {
                    descriptionId += $"{segments[i]}-";
                }
                i++;
            }
            return descriptionId.TrimEnd('-').ToLower();
        }
    }
}
