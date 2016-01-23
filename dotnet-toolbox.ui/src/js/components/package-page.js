var React = require('react');
import { Link } from 'react-router'
var $ = require('jquery');

var Details = React.createClass({
  render() {
    return (<section>
      <blockquote><em>"{this.props.package.description}"</em></blockquote>
      <p><strong>owners: </strong>{this.props.package.owners}</p>
      <p><strong>version: </strong>{this.props.package.version}</p>
    </section>);
  }
});

var PackagePage =
  React.createClass({
    getInitialState() {
      return {
        package: null
      }
    },
    load(packageName) {
      $.ajax({
        type: 'get',
        url: `/api/packages/${packageName}`,
        contentType: 'application/json'
      }).then(data=> {
        this.setState({package: data});
      });
    },
    componentWillReceiveProps(newProps) {
      this.load(newProps.params.packageName);
    },

    componentDidMount() {
      this.load(this.props.params.packageName);
    },
    render() {
      return (
        <div>
          <div className="h3">{this.props.params.packageName}</div>
          {
            (() => {
              if (this.state.package) {
                return (<Details package={this.state.package}/>);
              }
            })()
          }
          <li><Link to="/">&lt;- Back</Link></li>
        </div>
      )
    }
  });

export default PackagePage;