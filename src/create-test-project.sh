#!/bin/bash

dotnet new xunit -o Moneybox.App.Tests
dotnet sln add Moneybox.App.Tests/Moneybox.App.Tests.csproj

dotnet add Moneybox.App.Tests/Moneybox.App.Tests.csproj reference Moneybox.App/Moneybox.App.csproj

dotnet add Moneybox.App.Tests/Moneybox.App.Tests.csproj package moq

