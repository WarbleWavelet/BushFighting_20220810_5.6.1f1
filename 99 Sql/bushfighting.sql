/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : bushfighting

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2022-08-18 15:02:59
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for result
-- ----------------------------
DROP TABLE IF EXISTS `result`;
CREATE TABLE `result` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `totalcount` int(11) DEFAULT '0',
  `wincount` int(11) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `userid` (`userid`) USING BTREE,
  CONSTRAINT `result_ibfk_1` FOREIGN KEY (`userid`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of result
-- ----------------------------
INSERT INTO `result` VALUES ('1', '23', '2', '1');
INSERT INTO `result` VALUES ('2', '25', '3', '2');
INSERT INTO `result` VALUES ('3', '26', '4', '2');
INSERT INTO `result` VALUES ('4', '45', '4', '4');
INSERT INTO `result` VALUES ('5', '46', '4', '0');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('23', '张飞', '1234');
INSERT INTO `user` VALUES ('24', '赵云', '12345678');
INSERT INTO `user` VALUES ('25', '黄忠', '123');
INSERT INTO `user` VALUES ('26', '马超', '123');
INSERT INTO `user` VALUES ('27', '魏延', '123');
INSERT INTO `user` VALUES ('28', '陈到', '12');
INSERT INTO `user` VALUES ('29', '马良', '123');
INSERT INTO `user` VALUES ('30', '姜维', '123');
INSERT INTO `user` VALUES ('31', '费祎', '123');
INSERT INTO `user` VALUES ('32', '蒋琬', '123');
INSERT INTO `user` VALUES ('33', '马岱', '123');
INSERT INTO `user` VALUES ('34', '马铁', '123');
INSERT INTO `user` VALUES ('35', '关平', '123');
INSERT INTO `user` VALUES ('36', '关兴', '123');
INSERT INTO `user` VALUES ('37', '关定', '123');
INSERT INTO `user` VALUES ('38', '关银屏', '12');
INSERT INTO `user` VALUES ('39', '关统', '12');
INSERT INTO `user` VALUES ('40', '关索', '12');
INSERT INTO `user` VALUES ('41', '张绍', '12');
INSERT INTO `user` VALUES ('42', '诸葛瞻', '12');
INSERT INTO `user` VALUES ('43', '刘禅', '12');
INSERT INTO `user` VALUES ('44', '严颜', '123');
INSERT INTO `user` VALUES ('45', '1', '1');
INSERT INTO `user` VALUES ('46', '2', '2');
