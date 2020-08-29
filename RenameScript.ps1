# Credit goes to Kevin Brydon (https://stackoverflow.com/a/45648198/5187440)
# Change these three variables to suit your requirements
# $PSScriptRoot = Current Directory
$baseDirectory = $PSScriptRoot
$a = "CompanyName.ProjectName"
$b = "YourCompanyName.YourProjectName"

# Get all files
$files = Get-ChildItem $baseDirectory -File -Recurse
# Get all the directories
$directorys = Get-ChildItem $baseDirectory -Directory -Recurse

# Replace the contents of the files only if there is a match
foreach ($file in $files)
{
    $fileContent = Get-Content -Path $file.FullName

    if ($fileContent -match $a)
    {
        $newFileContent = $fileContent -replace $a, $b
        Set-Content -Path $file.FullName -Value $newFileContent
    }
}

# Change the names of the files first then change the names of the directories

# Iterate through the files and change their names
foreach ($file in $files)
{
    if ($file -match $a)
    {
        $newName = $file.Name -replace $a, $b
        Rename-Item -Path $file.FullName -NewName $newName
    }
}

# Reverse the array of directories so we go deepest first.
# This stops us from renaming a parent directory then trying to 
# rename a sub directory which will no longer exist.
# e.g.
# We might have a directory structure "C:\Rename\Rename"
# and the file array would be [ C:\Rename, C:\Rename\Rename ].
# Without reversing we'd rename the first directory to "C:\NewName"
# and the directory structure would be "C:\NewName\Rename"
# and we'd then try to rename C:\Rename\Rename which would fail.
[array]::Reverse($directorys)

# Iterate through the directories and change their names
foreach ($directory in $directorys)
{
    if ($directory -match $a)
    {
        $newName = $directory.Name -replace $a, $b
        Rename-Item -Path $directory.FullName -NewName $newName
    }
}
