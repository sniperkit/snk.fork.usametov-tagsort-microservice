/*
Sniperkit-Bot
- Status: analyzed
*/

export const mocked_bookmarks = [{ 
 id:"6EE67F4C12280A5DCC6513DBF566726E"
,isPrivate:false
,addDate:"2016-11-21T08:05:34.0671476Z"
,description:"This solar air heating collector uses recycled aluminum soda pop cans for the absorber. The pop cans have the tops and bottoms drilled out, and are assembled into vertical columns that the air passes through."
,linkText:"How to build a pop can solar heater"
,linkUrl:"http://www.builditsolar.com/Projects/SpaceHeating/GregCanCol/GregCanCo.htm"
,tags:["green-economy", "solar"]
},
{
  id:"EA052233C0F81214E39688B33C1C529D"
  ,isPrivate:false
  ,addDate:"2016-11-21T08:05:34.0671476Z"
  ,description:"One of my favorite websites is builditsolar.com, a renewable energy site for DIYers. It's an excellent source of information about large grid-tied and off-grid PV systems. And it's a fascinating compendium of links to articles, free plans and Web sites of DIYers who are using smaller-scale PV systems to power their imaginations"
  ,linkText:"Ideas for DIY solar power projects."
  ,linkUrl:"http://community.familyhandyman.com/tfh_group/b/diy_advice_blog/archive/2011/09/06/diy-solar-power-projects.aspx"
  ,tags:["green-economy"]
}];

export default function bookmarksService(url) {
  return new Promise((resolve, reject) => {

    process.nextTick( 
      () => url ? resolve(mocked_bookmarks) : reject({ 
        error: "invalid url: " + url, })
    );
  });
}