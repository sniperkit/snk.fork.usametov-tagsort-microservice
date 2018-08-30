/*
Sniperkit-Bot
- Status: analyzed
*/

import React from 'react';
import ReactDOM from 'react-dom';

import App from './lib/App';

export default ReactDOM.render(
  <App />,
  document.getElementById('app') || document.createElement('div')
);

