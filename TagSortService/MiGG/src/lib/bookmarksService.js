export default function bookmarksService(url){
    return fetch(url)
    .then(res => { res.json();})        
    .catch(err => console.log("getData error: ", err));
}


