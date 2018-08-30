/*
Sniperkit-Bot
- Status: analyzed
*/

import React from 'react';
import Tag from './Tag';
// import arrow_up from '../../public/images/vote_up.png';
// import arrow_down from '../../public/images/vote_down.png';

const style = {
  story: {
    display: 'flex',
    flexBasis: 900,
    marginLeft: "5px",
    paddingLeft: "5px",
    borderTop: "1px solid #e4e6e8"
  },
  rank: {
    minWidth: "37px",
    fontSize: '15px',
    marginTop: "20px",
    textAlign: 'center',
    
    color: "#6a737c"
  },
  rank_count: {
    margin: "8px 0",
    display: "block",
    lineHeight: "1.3em",
  },
  vote_up: {
    height: "19px",
    //backgroundImage: `url(${arrow_up})`,
    backgroundRepeat: "no-repeat",
    fontSize: "1px",
    display: "block",
    margin: "2px",
  },
  vote_down: {
    height: "19px",
    //backgroundImage: `url(${arrow_down})`,
    backgroundRepeat: "no-repeat",
    fontSize: "1px",
    display: "block",
    margin: "2px",
  },
  title: {
    fontFamily: "Arial",
    fontWeight: "normal",
    fontSize: '16px',
    alignSelf: 'center',
    flexBasis: 700,
    color: "#195695",
    marginLeft: "5px",
    marginTop: "5px",
    marginBottom: "5px",
  },
  info: {    
    alignSelf: 'center',    
  },
  infoItem: {    
    fontSize: '15px',
    backgroundColor: "#FFF8DC",
    padding: "5px 5px 5px 10px",
  },
  post_tags: {
    marginTop: "10px",
    padding: 0,
    color: "#000",
    fontSize: "12px",
    fontFamily: "Verdana,sans-serif",
    listStyleType: "none",    
  },
  
};

export default function Bookmark(props) {
  const {id, description, linkText, linkUrl, tags} = props.bookmark;  
  return (
    <div style={style.story}>
      <div style={style.rank}>
        
          <a style={style.vote_up}>up</a>
          <span id={id} style={style.rank_count}> 77 </span>
          <a style={style.vote_down}>down</a>
        
      </div>
      <div style={style.title}>
        <h4 style={style.title}>
          <a href={linkUrl} rel="noopener noreferrer" target="_blank">
            <span>
            {linkText}
            </span>
          </a>
        </h4>
        <p style={style.infoItem}>{description}</p>
        <div style={style.post_tags}>
          <ul style={style.post_tags}>
            {tags.map(t => <Tag key={t} tag={t} />)}
          </ul>
        </div>        
      </div>
         
    </div>
    
  );
}
Bookmark.propTypes = {
  bookmark: React.PropTypes.object.isRequired,
};
