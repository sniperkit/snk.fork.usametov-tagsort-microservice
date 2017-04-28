import React from 'react';
import Bookmark from './Bookmark';

const style = {
  bookmarks: {
    display: 'flex',
    flexBasis: 900,    
  },
  
};

export default function Bookmarks(props) {
  console.log(props);
  
  return (    
    <div style={style} >
      <div>
      {props.bookmarks.map(bookmark => <Bookmark 
      key={bookmark.id} 
      bookmark={bookmark}
      />)}
      </div>
     

    </div>
  );
}
Bookmarks.propTypes = {
  bookmarks: React.PropTypes.array.isRequired,
};
