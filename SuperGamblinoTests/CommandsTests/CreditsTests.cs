﻿using Moq;
using SuperGamblino.CommandsLogics;
using SuperGamblino.DatabaseConnectors;
using Xunit;

namespace SuperGamblinoTests.CommandsTests
{
    public class CreditsTests
    {
        private CreditsCommandLogic GetCreditsCommandLogic(UsersConnector usersConnector)
        {
            return new CreditsCommandLogic(usersConnector, Helpers.GetMessages());
        }

        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(10000)]
        public async void IsCorrectAmountDisplayed(int amount)
        {
            var usersConnector = Helpers.GetDatabaseConnector<UsersConnector>();
            usersConnector.Setup(x => x.CommandGetUserCredits(0)).ReturnsAsync(amount);
            var logic = GetCreditsCommandLogic(usersConnector.Object);

            var result = await logic.GetCurrentCreditStatus(0);

            Assert.Contains(result.Fields, x => x.Name == "Credits balance");
            Assert.Equal("Credits balance", result.Fields[0].Name);
            Assert.Equal(amount.ToString(), result.Fields[0].Value);
        }
    }
}