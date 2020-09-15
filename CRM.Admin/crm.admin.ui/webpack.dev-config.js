/* global __dirname */
const webpack = require('webpack')
const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')

module.exports = {
	entry: [
		'babel-polyfill',
		'whatwg-fetch',
		'react-hot-loader/patch',
		'./src/index.js',
	],
	output: {
		filename: './build-dev/bundle.js'
	},
    devServer: {
        contentBase: path.join(__dirname, "build-dev"),
		port: 9000,
		historyApiFallback: true
    },
	plugins: [
		new webpack.optimize.OccurrenceOrderPlugin(),
		new webpack.optimize.AggressiveMergingPlugin(),
		//new BundleAnalyzerPlugin(),

        new HtmlWebpackPlugin({
			hash: true,
            filename: './build-dev/index.html' 
		}),
		new webpack.DefinePlugin({
			'process.env': { 
				NODE_ENV: JSON.stringify('development')
			}
		}),
	],
	module: {
		rules: [
			{
				test: /\.(js|jsx)$/,
				exclude: /node_modules/,
				loader: 'babel-loader'
			},
			{
				// include: /style/,
				test: /\.css$/,
				loader: 'style-loader!css-loader'
			},
			{
				test: /\.woff($|\?)|\.woff2($|\?)|\.ttf($|\?)|\.eot($|\?)|\.svg($|\?)/,
				loader: 'url-loader?limit=20000&name=[name]-[hash].[ext]',
			},
			{
				test: /\.(jpe?g|png|gif|svg)$/i,
				use: [
				  'url-loader?limit=10000',
				  'img-loader'
				]
			},
			{
				exclude: /node_modules/,
				loader: 'eslint-loader',
				test: /\.js$/
			},
		]
	},
	resolve: {
		modules: ['src', 'node_modules'],
		alias: {
			common: path.resolve(__dirname, 'src')
		}
	}
}
