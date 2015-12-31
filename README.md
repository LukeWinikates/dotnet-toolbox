![](https://travis-ci.org/LukeWinikates/dotnet-toolbox.svg?branch=master)
# .NET Toolbox

## Goals of this Project
1. As a product, provide a resource for .NET developers who want to find useful libraries for building .NET applications, especially FOSS libraries. When in doubt, do what ruby-toolbox.org does.
1. As a codebase, demonstrate patterns for building .NET applications with maximal developer productivity and happiness
  1. using the cross-platform `dnx` runtime
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
$ dnu restore
$ dnx -p dotnet-toolbox.api web
```

the `-p` flag specifies the path to the directory containing the .NET application, and is not required if you run `dnx web` from that directory directly

## Pushing to Cloud Foundry

The staging instance is here:

http://dotnet-toolbox-staging.cfapps.pez.pivotal.io/

To push, you will need to point your cf cli to the pez api endpoint

```
cf api https://api.run.pez.pivotal.io
```

To push to staging (somewhat awkward at the moment):

```
cd dotnet-toolbox.ui
gulp prepush
cd ..
cf push
```

If you are creating a new CF app, replace `dotnet-toolbox-staging` with an appname of your own choice
