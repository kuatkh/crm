/* global __dirname */
const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')

module.exports = {
	mode: 'development',
	context: __dirname,
	entry: './src/index.js',
	devtool: 'inline-source-map',
	output: {
		publicPath: '/',
		path: path.resolve(__dirname, 'dist'),
		filename: 'bundle.js',
		clean: true,
	},
	devServer: {
		historyApiFallback: true,
	},
	plugins: [
		//new BundleAnalyzerPlugin(),
		new HtmlWebpackPlugin({
			title: 'CRM. Admin. Development',
			filename: 'index.html',
			template: './index.html',
			xhtml: true,
		}),
	],
	module: {
		rules: [
			{
				test: /\.(js|jsx)$/,
				exclude: /node_modules/,
				use: ['babel-loader'],
			},
			{
				// include: /style/,
				test: /\.css$/,
				use: ['style-loader', 'css-loader'],
			},
			{
				test: /\.(jpe?g|png|gif|svg)$/i,
				use: [
					'url-loader?limit=10000',
					'img-loader',
				],
			},
		],
	},
	resolve: {
		modules: [path.resolve(__dirname, 'src'), 'node_modules'],
		alias: {
			common: path.resolve(__dirname, 'src'),
		},
		fallback: {
		  'fs': false,
		  'tls': false,
		  'net': false,
		  'path': false,
		  'zlib': false,
		  'http': false,
		  'https': false,
		  'stream': false,
		  'crypto': false,
		  'events': false, // require.resolve('events/'); npm i events
		  'url': false, // require.resolve('url/'); npm i url
		  'vm': false, // require.resolve("vm-browserify"); npm i vm-browserify
		  'tty': false, // require.resolve("tty-browserify"); npm i tty-browserify
		  'console': require.resolve('console-browserify'),
		  'constants': require.resolve('constants-browserify'),
		  'assert': false, // require.resolve("assert/"); npm i assert
		  'querystring': false, // require.resolve("querystring-es3"); npm i querystring-es3
		  'os': require.resolve('os-browserify/browser'), // require.resolve("os-browserify/browser"); npm i os-browserify
		  'crypto-browserify': require.resolve('crypto-browserify'), // require.resolve('crypto-browserify'); npm i rypto-browserify
		},
	}
}
