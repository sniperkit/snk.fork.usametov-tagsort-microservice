/*
Sniperkit-Bot
- Status: analyzed
*/

var bookmarksService = {
    fetchBookmarks: function(url){    
        return fetch(url, {credentials: 'same-origin'})
        .then(function(res) {console.log("fetched res", res); return res.json();})        
        .then(function(json){            
            //console.log("fetched json", json);
            return json;
        })  
        .catch(err => console.log("getData error: ", err));
    }
}

module.exports = bookmarksService;

