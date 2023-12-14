using Capstone.Exceptions;
using Capstone.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Linq;

namespace Capstone.DAO
{
  public class ApiDao : IApiDao
  {

    private static HttpClient ncbiClient = new()
    {
      BaseAddress = new Uri("https://eutils.ncbi.nlm.nih.gov/entrez/eutils/"),
    };
    private string connectionString;

    public ApiDao(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public async Task<string> NCBIApiGetProteinID(string name)
    {
      HttpResponseMessage response = new HttpResponseMessage();
      string id = "";
      try
      {
        using (response = await ncbiClient.GetAsync($"esearch.fcgi?db=protein&term={name}"))
        {

          response.EnsureSuccessStatusCode();
          string list = await response.Content.ReadAsStringAsync();
          id = ParseNCBIProteinId(list);

        }
      }
      catch (HttpRequestException ex)
      {
        throw new DaoException("HTTP exception occurred", ex);
      }
      return id;
    }
    public async Task<Protein> NCBIApiGetProteinSequence(string id)
    {
      Protein protein = new Protein();
      try
      {
        using (HttpResponseMessage response = await ncbiClient.GetAsync($"efetch.fcgi?db=protein&id={id}&rettype=fasta&retmode=text"))
        {
          response.EnsureSuccessStatusCode();
          string fasta = await response.Content.ReadAsStringAsync();
          protein = ParseNCBIFasta(fasta);
        }
      }
      catch (HttpRequestException ex)
      {
        throw new DaoException("HTTP exception occurred", ex);
      }
      return protein;
    }
    public async Task<Protein> RCSBApiGetProteinSequence(string id)
    {
      Protein protein = new Protein();
      try
      {
        var searchQuery = new
        {
          request_info = new
          {
            query = id
          },
          request_options = new
          {
            return_type = "polymer_entity"
          }
        };


        var jsonPayload = JsonSerializer.Serialize(searchQuery);

        var encodedJsonPayload = Uri.EscapeDataString(jsonPayload);
        using (HttpResponseMessage response = await new HttpClient().GetAsync($"https://data.rcsb.org/rest/v1/core/polymer_entity/{id.Replace("_", "/")}"))
        {
          response.EnsureSuccessStatusCode();
          string fasta = await response.Content.ReadAsStringAsync();
          protein = ParseRCSBFasta(fasta);
        }
      }
      catch (HttpRequestException ex)
      {
        throw new DaoException("HTTP exception occurred", ex);
      }
      return protein;
    }
    public async Task<string> RCSBApiGetProteinID(string name)
    {
      HttpResponseMessage response = new HttpResponseMessage();
      string id = "";
      try
      {

        var searchRequest = new
        {
          query = new
          {
            type = "terminal",
            service = "full_text",
            parameters = new
            {
              value = name,
            }
          },
          return_type = "polymer_entity"
        };

        var jsonPayload = JsonSerializer.Serialize(searchRequest);

        var encodedJsonPayload = Uri.EscapeDataString(jsonPayload);

        using (response = await new HttpClient().GetAsync($"https://search.rcsb.org/rcsbsearch/v2/query?json={encodedJsonPayload}"))
        {

          response.EnsureSuccessStatusCode();
          string list = await response.Content.ReadAsStringAsync();
          id = ParseRCSBProteinId(list);

        }
      }
      catch (HttpRequestException ex)
      {
        throw new DaoException("HTTP exception occurred", ex);
      }
      return id;
    }
    public static string ParseNCBIProteinId(string xmlData)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(xmlData);

      string idListContent = xmlDoc.SelectSingleNode("//Id")?.InnerXml;

      return idListContent;
    }
    public static string ParseRCSBProteinId(string xmlData)
    {
      JsonDocument jsonObject = JsonSerializer.Deserialize<JsonDocument>(xmlData);

      List<string> pdbIds = jsonObject?.RootElement
          .GetProperty("result_set")
          .EnumerateArray()
          .Select(entity => entity.GetProperty("identifier").GetString())
          .ToList();

      return pdbIds[0];
    }

    public Protein ParseNCBIFasta(string fastaData)
    {
      string[] lines = fastaData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
      string proteinId = lines[0].Trim().Substring(1);

      StringBuilder sequenceBuilder = new StringBuilder();
      for (int i = 1; i < lines.Length; i++)
      {
        sequenceBuilder.Append(lines[i].Trim());
      }

      string proteinSequence = sequenceBuilder.ToString();

      return new Protein
      {
        Description = proteinId,
        ProteinSequence = proteinSequence
      };
    }
    public Protein ParseRCSBFasta(string fastaData)
    {
      JsonDocument jsonObject = JsonSerializer.Deserialize<JsonDocument>(fastaData);
      // Extract the PDB IDs from the response
      string sequence = jsonObject?.RootElement
          .GetProperty("entity_poly")
          .GetProperty("pdbx_seq_one_letter_code").GetString();
      string description = jsonObject?.RootElement
         .GetProperty("rcsb_polymer_entity")
         .GetProperty("pdbx_description").GetString();
      return new Protein
      {
        Description = description,
        ProteinSequence = sequence
      };
    }
  }
}
