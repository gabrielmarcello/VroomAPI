using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using VroomAPI.DTOs;
using VroomAPI.Model.Enum;

namespace VroomAPI.Test
{
    public class MotoTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly string? _apiKey;

        public MotoTest(WebApplicationFactory<Program> factory)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            _apiKey = config["Authentication:ApiKey"];
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetMotos_ReturnListOfMotos()
        {

            //Arrange
            _client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

            //Act
            var response = await _client.GetAsync("/moto");

            //Assert
            var motos = await response.Content.ReadFromJsonAsync<PagedResponse<MotoDto>>();

            Assert.NotNull(motos);
        }

        [Fact]
        public async Task PostMotos_ReturnCreatedStatusCode()
        {
            //Arrange
            _client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
            var motoDto = new CreateMotoDto()
            {
                Placa = "MOT7G28",
                Chassi = "9BG116GW04C400001",
                DescricaoProblema = "Problema no cabeçote",
                ModeloMoto = ModeloMoto.MOTTUSPORT,
                CategoriaProblema = CategoriaProblema.MECANICO,
                TagId = 1
            };

            //Act
            var response = await _client.PostAsJsonAsync<CreateMotoDto>("/moto", motoDto);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdMoto = await response.Content.ReadFromJsonAsync<MotoDto>();
            Assert.NotNull(createdMoto);
            Assert.Equal(motoDto.Placa, createdMoto.Placa);
            Assert.Equal(motoDto.Chassi, createdMoto.Chassi);

        }
    }
}