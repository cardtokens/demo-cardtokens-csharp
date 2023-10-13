# demo-cardtokens-csharp

## Introduction
This example shows how to create a token towards the Cardtokens API, create a cryptogram, get status and delete the Token. 

You can run this code directly using a predefined apikey, merchantid and certificate. You can also get a FREE test account and inject with your own apikey, merchantid and certificate. Just visit https://www.cardtokens.io

## Steps to use this example code on Ubuntu

### Clone repo
```bash
git clone https://github.com/cardtokens/demo-cardtokens-csharp.git
```

### Navigate to folder locally
```bash
cd demo-cardtokens-csharp
```

### Install .net
#### Start with update
```bash
sudo apt-get update
sudo apt-get upgrade
```

#### Install dependencies
```bash
sudo apt-get install apt-transport-https dirmngr
```

#### Add microsoft repo
```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

#### Install .NET SDK/Runtime 7x
```bash
sudo apt-get install -y dotnet-sdk-7.0
```

### Build and run the app
#### Build
```bash
dotnet build
```

#### Run the program
```bash
dotnet run
```

