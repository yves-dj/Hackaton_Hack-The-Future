using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

// Swagger
// https://app-htf-2022.azurewebsites.net/swagger/index.html
// De httpclient die we gebruiken om http calls te maken
var client = new HttpClient();

// De base url die voor alle calls hetzelfde is
client.BaseAddress = new Uri("https://app-htf-2022.azurewebsites.net");

// De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNCIsIm5iZiI6MTY2ODUwMzUzOSwiZXhwIjoxNjY4NTg5OTM5LCJpYXQiOjE2Njg1MDM1Mzl9.xVM4l0t-btk2wEUwNmlyF3ZcCdzkcqopJ81omgSlsrItE58GQ2szXwbPb2aER-W1PPjIY0x1mMbaLY0MKzm_Sg";

// We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

// De url om de challenge te starten
var startUrl = "api/path/2/easy/Start";

// We voeren de call uit en wachten op de response
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
// De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
// Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
var startResponse = await client.GetAsync(startUrl);

// De url om de sample challenge data op te halen
var sampleUrl = "api/path/2/easy/Sample";

// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
var sampleGetResponse = await client.GetFromJsonAsync<List<String>>(sampleUrl);

sampleGetResponse.ForEach(Console.WriteLine);

// Je zoekt het antwoord

string GetAnswer(List<string>? sampleGetResponse)
{
    if (sampleGetResponse == null)
    {
        return "No strings found.";
    }

    StringBuilder solution = new StringBuilder();

    

    

    foreach (var orgString in sampleGetResponse)
    {
        var distinctCharsPerString = GetDistinctChars(orgString);

        var sortedDictList = new List<Dictionary<char, int>>();

        var dictCountPerChar = GetCharsOccurence(orgString, distinctCharsPerString);
        var sortedDict = SortCharsByOccurence(dictCountPerChar);
        sortedDictList.Add(sortedDict);

        var tempstring = "";
        for (int i = 0; i < sortedDictList.Count(); i++)
        {
            
            foreach (KeyValuePair<char, int> item in sortedDictList[i])
            {
                tempstring += item.Key;
            }
        }
        solution.Append(tempstring);
        solution.Append(" ");
    }

    //foreach (KeyValuePair<char, int> charCount in dictCountPerChar)
    //{
    //    Console.WriteLine("Key: {0}, Value: {1}",
    //        charCount.Key, charCount.Value);
    //}

    Console.WriteLine(solution);

    return "Test";
}

var sampleAnswer = GetAnswer(sampleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var samplePostResponse = await client.PostAsJsonAsync<string>(sampleUrl, sampleAnswer);

// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();

Console.WriteLine(samplePostResponseValue);

string GetDistinctChars(string orgString)
{
    var distinctChars = new String(orgString.Distinct().ToArray());

    return distinctChars;
}

Dictionary<char, int> GetCharsOccurence(string originalSting, string distChars)
{
    Dictionary<char, int> dictCharOccurence = new Dictionary<char, int>();
    for (int i = 0; i < distChars.Length; i++)
    {
        char distChar = distChars[i];
        int count = originalSting.Count(f => (f == distChar));
        dictCharOccurence.Add(distChar, count);
    }

    return dictCharOccurence;
}

Dictionary<char, int> SortCharsByOccurence(Dictionary<char, int> dictCountPerChar)
{
    Dictionary<char, int> sortedDict = new Dictionary<char, int>();
    foreach (KeyValuePair<char, int> item in dictCountPerChar.OrderBy(key => key.Value))
    {
        sortedDict.Add(item.Key, item.Value);
    }
    return sortedDict;
}

//// De url om de puzzle challenge data op te halen
//var puzzleUrl = "api/path/1/easy/Puzzle";
//// We doen de GET request en wachten op de het antwoord
//// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
//var puzzleGetResponse = await client.GetFromJsonAsync<List<int>>(puzzleUrl);

//// Je zoekt het antwoord
//var puzzleAnswer = GetAnswer(puzzleGetResponse);

//// We sturen het antwoord met een POST request
//// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
//// De response die we krijgen zal ons zeggen of ons antwoord juist was
//var puzzlePostResponse = await client.PostAsJsonAsync<int>(puzzleUrl, puzzleAnswer);

//// Om te zien of ons antwoord juist was moeten we de response uitlezen
//// Een 200 status code betekent dus niet dat je antwoord juist was!
//var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();