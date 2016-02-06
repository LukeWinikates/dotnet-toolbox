var React = require('react');
import { Link } from 'react-router'
var $ = require('jquery');
var LeaderPanel = require('./layout').LeaderPanel;

var Details = React.createClass({
  render() {
    return (<section>
      <div className="h3">{this.props.package.id}</div>

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
        <LeaderPanel>
          {
            (() => {
              if (this.state.package) {
                return (<Details package={this.state.package}/>);
              }
            })()
          }
          <li><Link to="/">&lt;- Back</Link></li>
        </LeaderPanel>
      )
    }
  });

export default PackagePage;