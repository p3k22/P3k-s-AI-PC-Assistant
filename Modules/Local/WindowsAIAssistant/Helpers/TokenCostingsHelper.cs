namespace WindowsAIAssistant.Helpers;

public static class TokenCostingsHelper
{
   private static Dictionary<string, double> inputTokenCost =
      new Dictionary<string, double> {{"gpt-4o-mini", 0.00000015}, {"gpt-4o", 0.000005}, {"gpt-3.5-turbo", 0.000003}};

   private static Dictionary<string, double> outputTokenCost =
      new Dictionary<string, double> {{"gpt-4o-mini", 0.0000006}, {"gpt-4o", 0.000015}, {"gpt-3.5-turbo", 0.000006}};

   public static double InputTokenCost(int modelIndex)
   {
      if (modelIndex >= 0 && modelIndex < inputTokenCost.Count)
      {
         var valueAtIndex = inputTokenCost.ElementAt(modelIndex).Value;
         return valueAtIndex;
      }

      return 0;
   }

   public static double OutputTokenCost(int modelIndex)
   {
      if (modelIndex >= 0 && modelIndex < outputTokenCost.Count)
      {
         var valueAtIndex = outputTokenCost.ElementAt(modelIndex).Value;
         return valueAtIndex;
      }

      return 0;
   }
}