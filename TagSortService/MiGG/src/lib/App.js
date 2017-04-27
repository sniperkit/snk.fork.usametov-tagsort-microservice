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
    flexBasis: 900,
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
          <NavLink to="/recycling" activeStyle={{ color: 'green' }}>Recycling</NavLink>{' '}
          <NavLink to="/life-style" activeStyle={{ color: 'green' }}>Life Style</NavLink>{' '}
          <NavLink to="/kids" activeStyle={{ color: 'green' }}>Kids</NavLink>{' '}
        </div>        
        <div style={style.bookmarks}>          
          <Route path="/recycling" component={() => (<BookmarkList url={this.getBookmarksUrl("recycling",0,10)} />) } />
          <Route path="/life-style" component={() => (<BookmarkList url={this.getBookmarksUrl("life-style",0,10)} />) } />
          <Route path="/kids" component={() => (<BookmarkList url={this.getBookmarksUrl("kids",0,10)} />) } />
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

