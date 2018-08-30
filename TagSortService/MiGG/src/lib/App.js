/*
Sniperkit-Bot
- Status: analyzed
*/

import React from 'react';
import {
  BrowserRouter as Router,
  Route,
  NavLink
} from 'react-router-dom';

import BookmarkList from './BookmarkList';

import * as constants from './constants';

const style = {
  root: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'space-around',
    width: 900,
  },
  title: {
    flexBasis: 500,
    alignSelf: 'center',
    padding: 10,
  },
  filter: {
    flexBasis: 400,
    alignSelf: 'center',

  },
  bookmarks: {
    flexBasis: 600,
    alignSelf: 'center',
  },
  side_bar: {
    float: "right",
    flexBasis: 200,
    textAlign: 'center',
  }
};

class App extends React.Component {  

  getBookmarksUrl(route,skip,limit){
      return `${constants.BASE_URL}/bookmarksByTagBundle/${route}/${skip}/${limit}/`;
  }  

  render() {
    return (
      <Router>
      <div style={style.root}>
        <div style={style.title}>
          <h1>.:|:. Migg Content Aggregator</h1>
          {/* TODO: add menu and routes */}
          <NavLink to="/bookmarks/recycling" activeStyle={{ color: 'green' }}>Recycling</NavLink>{' '}
          <NavLink to="/bookmarks/life-style" activeStyle={{ color: 'green' }}>Life Style</NavLink>{' '}
          <NavLink to="/bookmarks/kids" activeStyle={{ color: 'green' }}>Kids</NavLink>{' '}
        </div>        
        <div style={style.bookmarks}>                    
          <Route path="/bookmarks/recycling" component={() => (<BookmarkList url={this.getBookmarksUrl("recycling",0,10)} />) } />
          <Route path="/bookmarks/life-style" component={() => (<BookmarkList url={this.getBookmarksUrl("life-style",0,10)} />) } />
          <Route path="/bookmarks/kids" component={() => (<BookmarkList url={this.getBookmarksUrl("kids",0,10)} />) } />
          <Route path="/bookmarks/" component={() => (<BookmarkList url={this.getBookmarksUrl("recycling",0,10)} />) } />
          <Route path="/bookmarks" component={() => (<BookmarkList url={this.getBookmarksUrl("recycling",0,10)} />) } />
        </div>
         <div style={style.side_bar}>
          side bar goes here
        </div>   
      </div>
      </Router>
    );
  }
}

export default App;

