import React from 'react';
import ReactDOM from 'react-dom';
//import { shallow } from 'enzyme';
import Index from './index';

it('snapshot testing', () => {
  
  expect(JSON.stringify(
  Object.assign({}, Index, { _reactInternalInstance: 'censored' })
    )).toMatchSnapshot();
});

//TODO: do we need this?
// it('renders without crashing', () => {
  
//   //shallow(<App />);
//   //const div = document.createElement('div');
//   //div.setAttribute('id', 'app');
  
//   ReactDOM.render(<App />, document.getElementById('app'));
// });
