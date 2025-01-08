﻿namespace GoogleTextToSpeech.Services;

public static class GoogleApiKey
{
   private static readonly string envKey = "";

   public static string? GetKey
   {
      get
      {
         try
         {
            //return Environment.GetEnvironmentVariable("envKey");
            return Environment.GetEnvironmentVariable(envKey);
         }
         catch (Exception)
         {
            return string.Empty;
         }
      }
   }
}