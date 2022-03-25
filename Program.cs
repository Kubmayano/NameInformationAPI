using Newtonsoft.Json;

/*I know that this program is super simple to make but goddamn its like my second day of actually getting to know API's lmfao so give me a break thank you <3*/

namespace NameInformation
{
    class hasMain
    {
        public static async Task FetchInformation(string name)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage nameAge = await client.GetAsync($"https://api.agify.io/?name={name}");
            HttpResponseMessage nameGender = await client.GetAsync($"https://api.genderize.io/?name={name}");
            HttpResponseMessage nameNationality = await client.GetAsync($"https://api.nationalize.io/?name={name}");

            string nameData = await nameAge.Content.ReadAsStringAsync();
            string genderData = await nameGender.Content.ReadAsStringAsync();
            string nationalityData = await nameNationality.Content.ReadAsStringAsync();

            await File.WriteAllTextAsync("nameData.json", FormatJson(nameData));
            await File.WriteAllTextAsync("genderData.json", FormatJson(genderData));
            await File.WriteAllTextAsync("nationalityData.json", FormatJson(nationalityData));
        }

        public static string[] DisplayData()
        {
            string[] returnData = new string[3];
            string data = File.ReadAllText("nameData.json");
            dynamic nameData = JsonConvert.DeserializeObject(data);
            data = File.ReadAllText("genderData.json");
            dynamic genderData = JsonConvert.DeserializeObject(data);
            data = File.ReadAllText("nationalityData.json");
            dynamic nationalityData = JsonConvert.DeserializeObject(data);

            if (nameData != null && genderData != null && nationalityData != null)
            {
                returnData[0] = $"Name: {nameData["name"]} | Age: {nameData["age"]}";
                double probability = Convert.ToDouble(genderData["probability"]) * 100;
                returnData[1] = $"Gender: {genderData["gender"]} | Probability: {probability}%";
                probability = Convert.ToDouble(nationalityData["country"][0]["probability"]) * 100;
                returnData[2] = $"Nationality: {nationalityData["country"][0]["country_id"]} | Probability: {probability}%";
            }

            return returnData;
        }

        public static string FormatJson(string data)
        {
            dynamic parsedData = JsonConvert.DeserializeObject(data);
            return JsonConvert.SerializeObject(parsedData, Formatting.Indented);
        }

        public static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            System.Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            if (name != null)
            {
                await FetchInformation(name);

                Console.Clear();

                foreach(object obj in DisplayData())
                {
                    System.Console.WriteLine(obj);
                }
            }
        }
    }
}