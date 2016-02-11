import React from 'react';

var LandingPage = require('./components/landing-page');
var PackagePage = require('./components/package-page');
var {IntlProvider}  = require('react-intl');

const App = React.createClass({
  render() {
    return (
      <IntlProvider locale="en">
        <div>
          {this.props.children}
        </div>
      </IntlProvider>
    )
  }
});

const routes = {
  path: '/',
  component: App,
  indexRoute: {component: LandingPage},
  childRoutes: [
    {path: 'package/:packageName', component: PackagePage}
  ]
};


export default routes;