/*
Sniperkit-Bot
- Status: analyzed
*/

﻿//keys in state transtions dictionary represent element ids (see select boxes below) 
//to which we hook our keyup event handlers
const get_states_transitions = () => {
        let states_dict = new Map();
        states_dict.set('#tagBundleList', {/*right arrow pressed*/'39': ["topTags", "freqTags"] });
        states_dict.set('#freqTagsList', { /*left arrow pressed*/ '37': ["freqTags", "topTags"], '39': ["freqTags", "exclTags"] });
        states_dict.set('#excludeTagsList',{/*left arrow pressed*/ '37': ["exclTags", "freqTags"] })
        return states_dict;
}

const editTagBundle = (state, action) => {
    switch (action.type){
        case 'INIT_FREQ_TAGS_MODEL':
            return {
                states_transition_matrix : get_states_transitions()
                ,
                buffer_size : 1000                
                , 
                LoginRequired : true
                ,
                topTags : []
                ,
                freqTags : []
                , 
                exclTags : []
            };
            
        case 'MOVE':
            //src array and target array names are specified in states transition matrix
            let sourceArrayRef = state.states_transition_matrix.get(action.srcId)[action.keyCode][0];
            let targetArrayRef = state.states_transition_matrix.get(action.srcId)[action.keyCode][1];
            //move tag from source array to target array on (left or right arrow) key press
            let res = move(state[sourceArrayRef], state[targetArrayRef], action.slctValue);

            state[sourceArrayRef] = res.arrSrc;
            state[targetArrayRef] = res.arrTrg;
            state.val2focus = res.val2focus ? res.val2focus.tag : null;

            return state;

        case 'SET_TAG_BUNDLE':
            let exclTagBundlesStr = typeof action.data.excludeTagBundles === 'undefined' ? ''
                                         : action.data.excludeTagBundles.join(',');
            return Object.assign
                            (state, {                
                                        topTags : action.data.tags
                                        ,
                                        exclTags : action.data.excludeTags
                                        ,
                                        exclTagBundles : exclTagBundlesStr
                                    });           
                
        default: 
            return state;
    }
}
        
    // move val from src array to target array returning next value in src array to focus
    const move = function (arrSrc, arrTrg, val) {

        const nextVal2focus = getNextVal(arrSrc, val);
        const newSrc = arrSrc.filter(t=> t.tag !== val);
        const termCount = arrSrc.filter(t=> t.tag === val);
        //and add it to target array            
        arrTrg = termCount.concat(arrTrg);

        return { arrSrc: newSrc, arrTrg: arrTrg, val2focus: nextVal2focus };
    };
    
    //get index of value in src array, then use this index to get next value
    const getNextVal = function (arrSrc, term) {
        const fndIdx = getIdx(arrSrc, term);
        const result = fndIdx > 0 ? arrSrc[fndIdx - 1] :
                arrSrc.length > 1 ? arrSrc[fndIdx + 1] : null;

        return result;
    };
    
    const getIdx = function (arrSrc, val) {
        return arrSrc.findIndex(t => t.tag === val);
    };
   
    

