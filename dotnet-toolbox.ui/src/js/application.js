import React from 'react';

var LandingPage = require('./components/landing-page');
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
  indexRoute: { component: LandingPage },
  childRoutes: [
    { path: 'package/:packageName', component: PackagePage }
  ]
};


export default routes;