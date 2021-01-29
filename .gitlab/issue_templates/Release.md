/label ~backstage
/label ~"wf::backlog"
/label ~"area::release/packaging"

## Before

- [ ] Make sure that the project files have the correct release version set
  - The `<Version>` element has to match the release tag (without the _v_ prefix).
- [ ] Update any Reductech dependencies to release versions
  - When building releases, the CI job will only restore packages from the production
    nuget. Therefore, if a library has any pre-release dependencies, the build stage will fail.
  - To check for updates
    - `dotnet list .\AutoTheoryGenerator\AutoTheoryGenerator.csproj package --outdated`
    - `dotnet list .\AutoTheoryGenerator.Tests\AutoTheoryGenerator.Tests.csproj package --outdated`
    - `dotnet list .\AutoTheoryGenerator.TestsBase\AutoTheoryGenerator.TestsBase.csproj package --outdated`
  - To update
    - `dotnet add .\AutoTheoryGenerator\AutoTheoryGenerator.csproj package <name>`
    - `dotnet add .\AutoTheoryGenerator.Tests\AutoTheoryGenerator.Tests.csproj package <name>`
    - `dotnet add .\AutoTheoryGenerator.TestsBase\AutoTheoryGenerator.TestsBase.csproj package <name>`
- [ ] Update the changelog
  - Manually, or use: reductech/pwsh/New-Changelog>
  - To include all issues from the last release tag to _HEAD_:
    `New-Changelog.ps1 -ProjectId 21478876 -ReleaseVersion <insert version>`
- [ ] Update the readme / documentation with any new changes

## Create Release

- [ ] Go to Repository > Tags > New Tag
  - Tag name: v0.1.0
  - _Message_ and _Release Notes_ should be the same: brief description of the release
    and any major (especially breaking) changes, and a link to the `CHANGELOG.md`.

## After

- [ ] Attach packages to the release
  - Wait for the release pipeline to finish
  - Go to Project Overview > Releases
  - Click on edit (pencil in the top-right) for the new release
  - In the _Release assets_ section, add a new link for each package job artifact:
    - URL: Link to the package job artifacts. e.g. https://gitlab.com/reductech/utilities/autotheorygenerator/-/jobs/970202187/artifacts/download?file_type=archive
    - Link title: `Reductech.Utilities.AutoTheoryGenerator-v0.2.1.dll.zip`
    - Type: _Package_
  - Yes, this should and will be automated. Soonish.
- [ ] Increment minor version for all the projects.
  - Create a new MR
  - Update the `<Version>` element in the csproj files
  - Merge into master
