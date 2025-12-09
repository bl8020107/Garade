const Dotenv = require('dotenv-webpack');

module.exports = {
  plugins: [
    new Dotenv({
      path: './.env',
      safe: false,
      systemvars: true
    })
  ]
};

