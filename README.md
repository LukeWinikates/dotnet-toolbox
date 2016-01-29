[![Build Status](https://travis-ci.org/dotnet-toolbox/dotnet-toolbox.svg)](https://travis-ci.org/dotnet-toolbox/dotnet-toolbox)
# .NET Toolbox

## Goals of this Project
1. As a product, provide a resource for .NET developers who want to find useful libraries for building .NET applications, especially FOSS libraries. When in doubt, do what ruby-toolbox.org does.
1. As a codebase, demonstrate patterns for building .NET applications with maximal developer productivity and happiness
  1. using the cross-platform `dnx` runtime (and eventually the `dotnet` cli that will replace it) 
  1. using the 12-factor style and deploying to a PaaS 
  1. using Nodejs-based tooling for HTML, CSS, and JavaScript to reduce incidental complexity
  1. using a C# editor available on non-Windows platforms (VS Code)

## Getting Started

To build the ui project, you'll need Node.js and npm. You can install node via Homebrew
 
`brew install node` 
 
Or using [nvm](https://github.com/creationix/nvm)

Once that's done, you'll need to install the gulp command line utility

`npm install -g gulp` 
 
 and the npm dependencies of the ui project
 
 `cd dotnet-toolbox.ui; npm install`. 
 
To build the API project, refer to Microsoft's document on how to install the DNX runtime:

http://docs.asp.net/en/latest/getting-started/index.html

This app is built on Mac OSX and the production instance is hosted on a linux virtual machine.

Once you have the `dnu` and `dnx` executables on your path run these commands:

```
$ brew install redis # if you don't already have it
$ dnvm use 1.0.0-rc1-update1 -r corclr # See "coreclr vs mono" below
$ dnu restore
$ dnx -p src/dotnet-toolbox.api web
```

the `-p` flag specifies the path to the directory containing the .NET application, and is not required if you run `dnx web` from that directory directly

## Running Tests

Jasmine:

headlesly with: `$ gulp --gulpfile dotnet-toolbox.ui/gulpfile.js jasmine-phantom`

or in a browser with: `$ gulp --gulpfile dotnet-toolbox.ui/gulpfile.js jasmine` at good old http://localhost:8888

xUnit (C# API)


```
$ dnvm use 1.0.0-rc1-update1 -r mono # see "coreclr vs mono" below

$ dnx -p test/dotnet-toolbox.api.tests test
```

## Coreclr vs Mono
.NET and C# were originally introduced built to run on Windows, and for a long time Windows was the only supported operating system.
Recently Microsoft announced plans to introduce a smaller cross-platform version of the .NET runtime called `coreclr`.
Coreclr is a new project, but there is also a mature, open-source, cross-platform reimpementation of the larger .NET framework called `Mono`.

The dotnet-toolbox REST API is written in C# and runs on linux and OSX using the coreclr runtime, but the tests make use of a mocking library, `Moq`, 
that has not yet been fully ported to coreclr [https://github.com/Moq/moq4/issues/168](https://github.com/Moq/moq4/issues/168). As a result, the tests currently *must* be run under mono and may crash or hang if run under coreclr.

Unfortunately, the `StackExchange.Redis` client used for persistence has a *Mono-specific* bug that prevents it from connecting to a redis instance. That means that while the tests run under Mono, the web api does not currently work under Mono, which is super awkward. To work around this, use the `dnvm use <version>` command to switch the current bash session between different runtimes (as indicated in the code snippets above). Keeping one terminal window open for running the web api and another for running the tests seems to work well.

These are community growing pains on the way to standardizing around a cross-platform .NET implementation and will likely be fixed in the months to come, rendering this section obsolete!

## So the data's all in Redis. That's an interesting choice.
The datastore for this app is Redis. This wasn't actually an intentional decision. The original plan was to use a relational database, but the only relational database available on PCF at the moment is MySQL, and the prerelease, CoreCLR-compatible version of Microsoft's EntityFramework (EF) ORM does not yet support MySQL. Earlier versions of EF do support MySQL, but don't work nicely with `dnx` or CoreCLR, so the easist way forward was trying something else entirely. After dabbling with other options for a while, Redis seemed like an option worth trying, especially in the spirit of experimentation.

## Deploying to Cloud Foundry
The staging instance is here:

http://dotnet-toolbox-staging.cfapps.pez.pivotal.io/

To push, you will need to point your cf cli to the pez api endpoint

```
cf api https://api.run.pez.pivotal.io
```

To push to staging:

```
$ gulp --gulpfile dotnet-toolbox.ui/gulpfile.js jasmine-phantom
$ cf push
```

This deployment uses the aspnet5 [community buildpack](https://github.com/cloudfoundry-community/asp.net5-buildpack), which is specified in manifest.yml

## ASP.NET 5 reference

For a good starting point to learn about ASP.NET 5, check out Microsoft's documentation [here](http://docs.asp.net/en/latest/conceptual-overview/index.html)
