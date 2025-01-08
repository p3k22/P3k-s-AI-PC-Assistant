namespace ChatGPT.Models
{
   public static class GptLlm
   {
      private static readonly Dictionary<int, string> Models =
         new Dictionary<int, string> {{0, "gpt-4o-mini"}, {1, "gpt-4o"}, {2, "gpt-3.5-turbo"}};

      public static string GetModelByIndex(int modelIndex)
      {
         if (modelIndex >= 0 && modelIndex < Models.Count)
         {
            var valueAtIndex = Models.ElementAt(modelIndex).Value;
            return valueAtIndex;
         }

         return "gpt-4o-mini";
      }
   }
}
