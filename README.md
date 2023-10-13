# demo-cardtokens-csharp

## Introduction
This example shows how to create a token towards the Cardtokens API, create a cryptogram, get status and delete the Token.

## Steps to use this example code on Ubuntu

### Clone repo
git clone https://github.com/cardtokens/demo-cardtokens-csharp.git


### navigate to folder locally
cd demo-cardtokens-csharp

### Install .net
#### start with update
sudo apt-get update
sudo apt-get upgrade

#### Install dependencies
sudo apt-get install apt-transport-https dirmngr

#### Add microsoft repo
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

#### Install .NET SDK/Runtime 7x
sudo apt-get install -y dotnet-sdk-7.0

### Build and run the app
#### build
dotnet build

#### Run the program
dotnet run

