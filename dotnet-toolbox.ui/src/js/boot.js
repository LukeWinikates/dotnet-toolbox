var ReactDOM = require('react/lib/ReactDOM');
var React = require('react');
var routes = require('./application');
import { Router, browserHistory } from 'react-router'

ReactDOM.render(React.createElement(Router, {
  routes: routes,
  history: browserHistory
}), document.getElementById('root'));