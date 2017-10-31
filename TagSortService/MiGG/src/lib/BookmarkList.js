import React from 'react';
import Bookmarks from './Bookmarks';
import bookmarksService from './bookmarksService';

export default class BookmarkList extends React.Component{
    
    constructor(props) {
      super(props);
      this.state = {
        bookmarks: []
      };    
    }
    
    componentDidMount() {      
        bookmarksService.fetchBookmarks(this.props.url)                  
                  .then((bookmarks) => 
                      { 
                          //console.log("logging bookmarks!", bookmarks);
                          this.setState({ bookmarks });                         
                      });
    }
    
    render (){
        return <Bookmarks bookmarks={this.state.bookmarks} />      
    }
}

 BookmarkList.propTypes = {    
    url: React.PropTypes.string.isRequired    
}