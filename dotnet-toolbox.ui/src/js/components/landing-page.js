var React = require('react');
import { Link } from 'react-router'
var $ = require('jquery');
var PackageAdder = require('./package-adder');
var Divider = require('pui-react-dividers').Divider;
var LeaderPanel = require('./layout').LeaderPanel;

var Category = React.createClass({
  render() {
    return (
      <section>
        <Divider/>
        <article className="container category">
          <h3>{this.props.category.title}</h3>
        </article>
      </section>
    )
  }
});

var LandingPage =
  React.createClass({
    componentDidMount() {
      $.ajax({
        type: 'get',
        url: '/api/categories',
        contentType: 'application/json'
      }).then(categories => {
        this.setState({categories: categories})
      });
    },
    render() {
      return (
        <div>
          <LeaderPanel>
            <PackageAdder/>
          </LeaderPanel>
          <h2 className="container">Libraries by Category</h2>
          {
            this.state && this.state.categories && this.state.categories.map((c)=> {
              return (<Category key={c.title} category={c}/>)
            })
          }
        </div>
      )
    }
  });

export default LandingPage;