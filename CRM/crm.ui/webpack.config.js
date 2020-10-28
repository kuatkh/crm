/* global __dirname */
const webpack = require('webpack')
var path = require('path')
var FileSystem = require('fs')
const CompressionPlugin = require('compression-webpack-plugin')
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const FaviconsWebpackPlugin = require('favicons-webpack-plugin')

module.exports = {
	entry: [
		'babel-polyfill',
		'whatwg-fetch',
		'react-hot-loader/patch',
		'./src/index.js',
	],
	optimization: {
		minimizer: [
			new UglifyJsPlugin(),
		],
	},
	output: {
		publicPath: './',
		path: path.resolve(__dirname, 'build'),
		filename: 'bundle-[hash].js',
	},
	devServer: {
		historyApiFallback: true,
	},
	plugins: [
		new webpack.optimize.OccurrenceOrderPlugin(),
		new webpack.optimize.AggressiveMergingPlugin(),
		new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
		new webpack.DefinePlugin({
			'process.env': {
				'NODE_ENV': JSON.stringify('production'),
			},
		}),
		new CompressionPlugin({
			filename: '[path].gz[query]',
			algorithm: 'gzip',
			test: /\.js$|\.css$|\.html$/,
			threshold: 10240,
			minRatio: 0.8,
		}),
		new HtmlWebpackPlugin({
			title: 'CRM',
			filename: 'index.html',
			template: 'index.html',
			xhtml: true,
		}),

		// https://github.com/itgalaxy/favicons#usage
		new FaviconsWebpackPlugin({
			logo: 'art-fraffrog-moy-sosed-totoro.jpg',
			prefix: 'icons-[hash]/',
			emitStats: false,
			statsFilename: 'iconstats.json',
			persistentCache: false,
			inject: true,
			background: '#fff',
			title: 'CRM',
			developerName: 'Kuat Khamitov',
			icons: {
				android: true,
				appleIcon: true,
				appleStartup: true,
				coast: false,
				favicons: true,
				firefox: true,
				opengraph: false,
				twitter: false,
				yandex: false,
				windows: true,
			},
		}),
		new webpack.DefinePlugin({
			'process.env': {
				NODE_ENV: JSON.stringify('production'),
			},
		}),
	],
	module: {
		rules: [
			{
				test: /\.(js|jsx)$/,
				exclude: /node_modules/,
				loader: 'babel-loader',
			},
			{
				test: /\.css$/,
				loader: 'style-loader!css-loader',
			},
			{
				test: /\.woff($|\?)|\.woff2($|\?)|\.ttf($|\?)|\.eot($|\?)|\.svg($|\?)/,
				loader: 'url-loader?limit=20000&name=[name]-[hash].[ext]',
			},
			{
				test: /\.(png|jpg|gif)$/,
				loader: 'file-loader',
			},
			{
				exclude: /node_modules/,
				loader: 'eslint-loader',
				test: /\.js$/,
			},
			{
				test: /\.html$/,
				loader: 'html-loader',
			},
		],
	},
	resolve: {
		modules: ['src', 'node_modules'],
		alias: {
			common: path.resolve(__dirname, 'src'),
		},
	},
}
