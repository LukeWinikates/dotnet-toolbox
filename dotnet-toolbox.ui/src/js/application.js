import React from 'react';

var PackageAdder = require('./components/package-adder');
var PackagePage = require('./components/package-page');

const App = React.createClass({
  render() {
    return (
      <div>
        {this.props.children}
      </div>
    )
  }
});

const routes = {
  path: '/',
  component: App,
  indexRoute: { component: PackageAdder },
  childRoutes: [
    { path: 'package/:packageName', component: PackagePage }
  ]
};


export default routes;