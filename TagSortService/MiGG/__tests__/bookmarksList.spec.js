import React from 'react'
import ReactDom from 'react-dom'
import TestUtils from 'react-addons-test-utils'

import * as constants from '../src/lib/constants';
import BookmarkList from '../src/lib/BookmarkList';
import  {default as bookmarksService, mocked_bookmarks } from '../src/lib/__mocks__/bookmarksService';

jest.mock('../src/lib/bookmarksService');

test('works with promises', () => {
    
    const div = document.createElement('div');
    //this could be any dummy
    const testUrl = `${constants.BASE_URL}/bookmarksByTagBundle/recycling/0/10`;  

    ReactDom.render(<BookmarkList url={testUrl} />, div);    
});



