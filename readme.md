<h2>Summary</h2>
<div>
   This is an api to retrieve a list of food trucks nearby a given latitude and longitude pulling from the https://data.sfgov.org/Economy-and-Community/Mobile-Food-Facility-Permit/rqzj-sfat/data  rss feed.
</div>

<br />
<br />
<h3>Jira system to track tasks</h3>
<div>
    <a href="https://greynault.atlassian.net/secure/RapidBoard.jspa?rapidView=1&projectKey=FOOD&view=planning.nodetail&issueLimit=100">https://greynault.atlassian.net</a>
</div>

<br />
<br />
<h3>Docker Hub</h3>
<div>
    <a href="https://hub.docker.com/repository/docker/greynault/foodtruck">https://hub.docker.com/repository/docker/greynault/foodtruck</a>
    Email 'greynault@gmail.com' to become a contributor.
</div>

<br />
<br />
<h3>Deployed as Azure Function</h3>
<div>
    <a href="https://greynaultfoodtruck.azurewebsites.net/api/FindFoodTrucks">https://greynaultfoodtruck.azurewebsites.net/api/FindFoodTrucks</a>
    This code has been deployed from the Docker Hub as an Azure Function.
</div>

<br />
<br />
<h3>Instructions</h3>
<div>
    <strong>https://greynaultfoodtruck.azurewebsites.net/api/FindFoodTrucks?longitude=1&latitude=1</strong>
    <br />
    Result will be the non-expired closest '5' food trucks to the given longitude and latiude.  Results are in JSON.
</div>

<br />
<br />
<h3>Developer Setup</h3>
<div>
    1.  The solution is setup for Visual Studio 2019 Community Edition.
    <br />
    2.  You'll need to get a Docker account, and request to be a contributer to the docker repository for this project.
    <br />
    3.  Become a member of the Jira project in order to track tasks.
    <br />
    4.  The admin will deploy the sql function to Azure from the docker repository.
    <br />
</div>

<br />
<br />
<h3>Notes</h3>
<div>
    I've been looking for an opportunity to create an Azure function, and I've been learning Docker, so this was a nice dovetail project.  
    <br />
    I was able to complete what i wished for the initial release as laid out in Jira, but there is plenty I wish I had been able to do, including:
    <br />
    1. Many more unit tests
    2. Integration Tests
    3. I setup a Bamboo instance, but didn't have time to setup a CI process
    4. I wanted to build a front-end, but knew i most likely wouldn't have the time.
 </div>
