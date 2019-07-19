# TODO LIST

**X** means **issue todo**.

**O** means **issue done**.

**?** means **issue disputed**.

## 7.17

O - 实现将strain拖拽至block的ui与逻辑

X - 修正摄像机移动僵硬的问题

O - 摄像机跑出地图边缘的问题

## 7.18

### X - 改动模型。
#### 1. 改动 coding gene的delta的运算：delta 可以编辑，具有加减乘除运算，并且可以获取block.chemical.count、strain.population、privateScource.chemical.count。

例如 ：某基因（PopA）的delta= strain.population×1.1+block.chemical.IPTG.count×private.ATP.count 意思是delta值由本细菌的人口+格子上的IPTG数量×自身含有的ATP数量运算得到

#### 2. 改动 coding gene规则：coding gene的改变物（product chemical、population、import chemical）都有各自的delta

#### 3. 改动regulatory gene 的 Condition：类似逻辑运算符，由and，or，正负，物质数量要求 四个要素组成，并且可以嵌套。

例如：(private.物质A>20 and private.物质B=0）or （map.物质C>100）×正：满足 (private.物质A>20 and private.物质B=0）or （map.物质C>100）时为true（下游coding gene工作），否则为false（下游coding gene不工作）

例如：(private.物质A>20 and private.物质B=0）or （map.物质C>100）×负：满足(物质A>20 or 物质B=0）and （物质C>100）时为false（下游coding gene不工作），否则为true（下游coding gene工作）

