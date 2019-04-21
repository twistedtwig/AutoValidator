using System.Collections.Generic;

namespace AutoValidator.Helpers
{
    public static class DictionaryHelperExtensions
    {
        public static void AddItemToList<T, TU>(this Dictionary<T, List<TU>> dictionary, T key, TU value)
        {
            var errorList = new List<TU>();

            if (dictionary.ContainsKey(key))
            {
                errorList = dictionary[key];
            }
            else
            {
                dictionary.Add(key, errorList);
            }

            errorList.Add(value);
            dictionary[key] = errorList;
        }
    }
}
