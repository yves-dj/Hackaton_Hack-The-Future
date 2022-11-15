﻿using Hackaton_Hack_The_Future;
using System.Net.Http.Headers;
using System.Net.Http.Json;

// Swagger
// https://app-htf-2022.azurewebsites.net/swagger/index.html
// De httpclient die we gebruiken om http calls te maken
var client = new HttpClient();

// De base url die voor alle calls hetzelfde is
client.BaseAddress = new Uri("https://app-htf-2022.azurewebsites.net");

// De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNCIsIm5iZiI6MTY2ODUwMzYyMywiZXhwIjoxNjY4NTkwMDIzLCJpYXQiOjE2Njg1MDM2MjN9.2f_aCgTzO32kWid1TFViwP-HySL3L5W6lh-b--Jod8IwEYuXHRO6vU6OkoPyruCCVywuBLPHSNtFDyPYLuSefg";

// We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

// De url om de challenge te starten
var startUrl = "api/path/1/easy/Start";

// We voeren de call uit en wachten op de response
// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
// De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
// Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
var startResponse = await client.GetAsync(startUrl);

// De url om de sample challenge data op te halen
var sampleUrl = "api/path/1/easy/Sample";

// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
var sampleGetResponse = await client.GetFromJsonAsync<List<string>>(sampleUrl);

string romanSum = GetAnswer(sampleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var samplePostResponse = await client.PostAsJsonAsync<string>(sampleUrl, romanSum);

// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();

Console.WriteLine(samplePostResponseValue);

// De url om de puzzle challenge data op te halen
var puzzleUrl = "api/path/1/easy/Puzzle";
// We doen de GET request en wachten op de het antwoord
// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
var puzzleGetResponse = await client.GetFromJsonAsync<List<string>>(puzzleUrl);

// Je zoekt het antwoord
var puzzleAnswer = GetAnswer(puzzleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var puzzlePostResponse = await client.PostAsJsonAsync<string>(puzzleUrl, puzzleAnswer);

// Om te zien of ons antwoord juist was moeten we de response uitlezen
// Een 200 status code betekent dus niet dat je antwoord juist was!
var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(puzzlePostResponseValue);
Console.ReadLine();

static string GetAnswer(List<string>? getResponse)
{
    List<int> convertedRomans = new List<int>();

    foreach (string item in getResponse)
    {
        int convertedNumeral = NumeralConvertor.RomanToInt(item);
        convertedRomans.Add(convertedNumeral);
    }

    int sumNumerals = convertedRomans.Sum();
    string romanSum = NumeralConvertor.IntToRoman(sumNumerals);
    Console.WriteLine(romanSum);
    return romanSum;
}