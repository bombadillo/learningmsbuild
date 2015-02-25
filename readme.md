# MS Build

Microsoft Build Engine, also known as MSBuild, is a build platform for managed code and was part of .NET Framework. Visual Studio depends on MSBuild, but MSBuild does not depend on Visual Studio.

## Targets

The `DefaultTargets` attribute in the `Project` node allows us to set default targets to execute.

In command line, we can specify the target to be invoked by using the parameter `/target:ProjectName`. We can also set parameters in a rsp file.
*Multiple targets can be specifed e.g. `/target:ProjectName,AnotherProject`*

### Invoking Multiple Targets

Multiple targets can be invoked via a few options:

* Using the commandline parameter `/t:TargetA;TargetC;etc`
* Add the `DefaultTargets` attribute with semi-colon delimited values e.g. `DefaultTargets="TargetA;TargetC"`
* Using `CallTarget` node within a target:


    </Target Name="TargetB">
        <CallTarget Targets="TargetA;TargetC" />
    </Target>
With `CallTarget` the targets are called after the current target has run.

With this method we can chain targets together by adding a `CallTarget` to each preceding target.

* `DependsOnTargets` will call any other target to run before the current target runs:


    <Target Name="ListTests" DependsOnTargets="ListSimpsons">

In this scenario, `ListSimpsons` will run before `ListTests`.

* `AfterTargets` added to a target will run that target after the `AfterTarget` has run.


    <Target Name="ReservedProps" AfterTargets="ListSimpsons">

`ReservedProps` will run after `ListSimpsons`.

* `BeforeTargets` is similar to `AfterTargets` but the current target will run before the `BeforeTargets` instead of after.

### Conditional Targets

We can use conditions to specify if the task is run or not.

    <PropertyGroup>
        <DoIt>Foo</DoIt>
    </PropertyGroup>
    <Target Name="TargetA" Condition="$(DoIt) === 'foo'">

In this example we created a property `DoIt` with the value `Foo`. The condition required `DoIt` to be `foo` for the task to run.

## Logging

We can set the logging verbosity by setting the Importance attribute `<Message Text="See ya, Joe!" Importance="Low"></Message>`
`<Message Text="See ya, Joe!" Importance="Normal"></Message>`
`<Message Text="See ya, Joe!" Importance="High"></Message>`

The versbosity of the logs can be set as a command line argument/rsp argument by using `/v:detailed`.
*Other settings are `diagnostic`,`minimal`, `normal`*

## Properties

Properties are defined within a property group:

    <PropertyGroup>
       <Name>Homer</Name>
     </PropertyGroup>

We can also reference properties within our XML as follows:

    <PropertyGroup>
       <Name>Homer</Name>
       <FullName>$(Name) Simpson</FullName>
    </PropertyGroup>

If we referenced `FullName` within our message:
`Text="Hello, $(FullName)!"`

This would output: `Hello, Homer Simpson!`

### Reserved Properties

These are basic properties about the runtime environment. To view all reserved properties set the log verbosity to `diagnostic`.


## Items
Items are similar to arrays. We initially define the item in a property group e.g.

    <PropertyGroup>
        <TestTexts>c:\Training\msbuild\tests\*.txt</TestTexts>
    </PropertyGroup>\*

We then specify the `Item` within an `ItemGroup`:

    <ItemGroup>
        <Tests Include="$(TestTexts)" />
    </ItemGroup>

This item will hold all text files within the tests directory. The item has access to meta data such as modified time which can be accessed as follows:

    <Target Name="ListTests">
        <Message Text="@(Tests)" />
        <Message Text="@(Tests->'%(ModifiedTime)')" />
    </Target>

We could specify our own meta data as follows:

    <ItemGroup>
        <Tests Include="$(TestTexts)">
            <Framework>Jasmine</Framework>
        </Tests>
    </ItemGroup>


## Importing Files

We can import files by using the following node:

    <Import Project="filename" />

We can use this to import targets, properties, etc.

## Target Inheritance

By using multiple files to seperate our project file, targets, properties, etc. it allows us to use inheritance with our Targets. If we have a target `TargetA` in a targets file we can inherit from it in our project file simply by creating a new target with the same name.  

## Building Visual Studio Solutions and Projects

Solution (.sln) files are not MSBuild scripts, however, they can still be executed by MSBuild as they are converted into in memory MSBuild scripts at run time.

Projects on the other hand are MSBuild scripts and no conversion is required.

### Editing Project files

.proj files have two targets which can be overwritten: `BeforeBuild` and `AfterBuild`. They run *before* and *after* the build.

## Extension Pack

The extension pack provides a collection of MSBuild tasks. This can be downloaded [here](http://mikefourie.github.io/MSBuildExtensionPack/).

The extension pack must be imported into the build script just as we would any other file:

        <Import Project="C:\Program Files\MSBuild\ExtensionPack\4.0\MSBuild.ExtensionPack.tasks" />

Change the above path to wherever the `MSBuild.ExtensionPack.tasks` file lives on your machine.

### 64-Bit CPU Issue

For 64-bit CPUs you may need to alter the `MSBuild.ExtensionPack.tasks` file. There will be a property as follows:

    <ExtensionTasksPath Condition="'$(ExtensionTasksPath)' == ''">$(MSBuildExtensionsPath)\ExtensionPack\4.0\</ExtensionTasksPath>

The property reference `$(MSBuildExtensionsPath)` must be changed to `$(MSBuildExtensionsPath64)`.
