var ReactDOM = require('react/lib/ReactDOM');
var React = require('react');
var PackageAdder = require('./components/package-adder');
ReactDOM.render(React.createElement(PackageAdder, null), document.getElementById('root'));