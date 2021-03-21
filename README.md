# Diploma-API
Repository to development API (Back End) for diploma work

How to manage migrations via PMC
Project (targetProject) and StartupProject must be "DL.EF.Migrations"
Example:
Add-Migration TestMigrationName -Project DL.EF.Migrations -StartupProject DL.EF.Migrations
Or you can set up default and startup projects through the GUI, then you can skip a passing 
the corresponding parameters to the command.