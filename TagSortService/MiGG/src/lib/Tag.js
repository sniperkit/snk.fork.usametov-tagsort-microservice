/*
Sniperkit-Bot
- Status: analyzed
*/

import React from 'react';
import * as constants from './constants';

const style = {
    tag: {
        fontFamily: "Arial",
        fontWeight: "normal",
        fontSize: '12px',
        float: "left"        
    },
    link:{
        textDecoration: "none",
        backgroundColor: "#cee0ed",
        padding: ".4em .5em"
    }

};

export default function Tag(props) {

    const tag = props.tag;
    
    return (
        <li style={style.tag}>
            <a style={style.link} href={`${constants.BASE_URL}/bookmarksByTags/${tag}`}>{tag}</a> 
        </li>
    );
}

Tag.propTypes = {
    tag: React.PropTypes.string.isRequired,
};