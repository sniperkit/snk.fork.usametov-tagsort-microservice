/*
Sniperkit-Bot
- Status: analyzed
*/

import React from 'react';
import { storiesOf, action, linkTo } from '@kadira/storybook';
import Bookmarks from '../lib/Bookmarks';

const bookmarks = [{ 
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
},
{
 id:"F30F8CF1B628E450AB422FCE6A524702"
,isPrivate:false
,addDate:"2016-11-21T08:05:34.0671476Z"
,description:"Standing tall like a giant sunflower in a sea of undulating prairie grasses, or in any rural setting, a windmill is a thing of beauty. Not only are water pumping windmills a joy to watch, but they are incredibly useful. Powered only by wind, they work like quietly-purring nonpolluting creatures, to keep our storage tanks overflowing with fresh water. They operate effortlessly, efficiently, reliably."
,linkText:"Water pumping windmills and how they work, including schematics"
,linkUrl:"http://www.backwoodshome.com/articles2/ainsworth90.html"
,tags:["green-economy"]
},
{
 id:"96E39FD577141C0178CE455EBD98B345"
,isPrivate:false
,addDate:"2016-11-21T08:05:34.0671476Z"
,description:"DIY Solar Pool Heater - Rob A\u0027s (Im)personal Blog. ... "
,linkText:"DIY Solar Pool Heater - Rob A\u0027s (Im)personal Blog."
,linkUrl:"http://www.silent9.com/blog/index.php?url=archives/107-DIY-Solar-Pool-Heater.html&serendipity[cview]=linear"
,tags:["green-economy"]
}];

storiesOf('Bookmarks', module)
  .add('bookmarks list', () => (
    <Bookmarks 
      bookmarks={bookmarks} 
      />
  ));
