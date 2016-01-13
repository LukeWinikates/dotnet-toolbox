var React = require('react');
import { Link } from 'react-router'

var PackagePage =
  React.createClass({
    render() {
      return (
        <div>
          <div className="h3">{this.props.params.packageName}</div>
          <li><Link to="/">&lt;- Back</Link></li>
          {this.props.children}
        </div>
      )
    }
  });

export default PackagePage;