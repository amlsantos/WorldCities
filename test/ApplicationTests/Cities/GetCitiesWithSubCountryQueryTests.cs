﻿using Application.Interfaces.Messaging;
using Application.Interfaces.Persistence;
using Application.UseCases.Cities.Queries.GetById;
using Application.UseCases.Cities.Queries.GetBySubCountry;
using CSharpFunctionalExtensions;
using Domain.Cities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApplicationTests.Cities;

public class GetCitiesWithSubCountryQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IQueryHandler<GetCitiesWithSubCountryQuery, Result<List<GetCityResponse>>> _handler;

    public GetCitiesWithSubCountryQueryTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetCitiesWithSubCountryQueryHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_FindsCitiesWithCountries_ReturnsSuccess()
    {
        // arrange
        var query = new GetCitiesWithSubCountryQuery();
        var repositoryResponse = Maybe.From<IEnumerable<City>>(new List<City>());
        
        _unitOfWork.Setup(u => u.Cities.GetBySubCountry(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsSuccess.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetBySubCountry(It.IsAny<string>()), Times.Once());
    }
    
    [Fact]
    public async Task Handle_DoesNotFindCitiesWithCountries_ReturnsFailure()
    {
        // arrange
        var query = new GetCitiesWithSubCountryQuery();
        var repositoryResponse = Maybe.From<IEnumerable<City>>(null);
        
        _unitOfWork.Setup(u => u.Cities.GetBySubCountry(It.IsAny<string>())).ReturnsAsync(repositoryResponse);
        
        // act
        var result = await _handler.Handle(query, CancellationToken.None);

        // assert
        result.IsFailure.Should().Be(true);
        _unitOfWork.Verify(u => u.Cities.GetBySubCountry(It.IsAny<string>()), Times.Once());
    }
}