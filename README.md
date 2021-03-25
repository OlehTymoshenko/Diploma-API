# Diploma-API
Repository to development API (Back End) for diploma work

<h4>How to manage migrations via PMC </h4>
Project (targetProject) and StartupProject must be "DL.EF.Migrations" <br>
Example: Add-Migration TestMigrationName -Project DL.EF.Migrations -StartupProject DL.EF.Migrations <br>
Or you can set up default and startup projects through the GUI, then you can skip a passing 
the corresponding parameters to the command.

<h4> How to build docker image </h4>
1) Open solution root folder (Diploma) <br>
2) run command: <br>
	<h6>docker build -f PL.Diploma.API/Dockerfile -t <YOUR_DOCKER_IMAGE_TAG(NAME)> .</h6>

<h4> How to deploy container to heroku </h4>
1) You should sign in Container registry
	<h6>heroku container:login</h6>
2) Push docker image with application. This command must be run from root folder of wepapi <br>
   project (Diploma/PL.Diploma.API)<br>
	<h6>heroku container:push -a <NAME_OF_YOUR_APP_IN_THE_HEROKU> web --context-path=../</h6>
3) Release the newly pushed docker images to deploy your app
	<h6>heroku container:release -a diploma-api-khai web</h6>