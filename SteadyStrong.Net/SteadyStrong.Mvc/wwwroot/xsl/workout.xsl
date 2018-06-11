<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Workout">
    <style>
      /* No real reason to put these in a separate .css file right now, since they are only used here
      and the XSL file would most likely be cached like a stylesheet if browser caching is enabled. */
      .exercise-instance { margin-bottom:30px; box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24); }
      .card-header h2 { text-transform:uppercase; }
      .meta > div > span { min-width:75px; display:inline-block; text-align:left;}
      .exercise-instance .table { margin-bottom:0; }
      .exercise-instance .card-body { padding: 0; }
    </style>
    <div class="row text-small">
      <h1 class="col-sm-12">
        <xsl:value-of select="Name"/>
      </h1>
      <p class="col-sm-12">
        Performed on
        <xsl:variable name="date" select="substring-before(CreatedTimestamp,'T')" />
        <xsl:variable name="time" select="substring-after(CreatedTimestamp,'T')" />
        <span class="font-weight-bold">
          <xsl:value-of select="concat(
                                    string(number(substring($date, 9, 2))), '/',
                                    string(number(substring($date, 6, 2))), '/',
                                    substring($date, 3, 2) 
                                    )" />
        </span>
        at
        <span class="font-weight-bold mr-3">
          <xsl:value-of select="substring($time, 0, 6)" />
        </span>
         Total weight lifted: <span class="font-weight-bold workout__total-weight"></span>
      </p>
    </div>
    <div class="row">
      <xsl:apply-templates select="ExerciseInstances" />
    </div>
    <div class="row">
      <div class="col">
        Total weight lifted: <span class="font-weight-bold workout__total-weight mr-3"></span>
        Total exercises: <span class="font-weight-bold">
          <xsl:value-of select="count(./ExerciseSets/ExerciseSet)" />
        </span>
      </div>
    </div>
    <!-- /row -->
  </xsl:template>
  <xsl:template match="ExerciseInstances">
    <xsl:apply-templates select="ExerciseInstance" />
  </xsl:template>
  <xsl:template match="ExerciseInstance">
    <div class="col-sm-12">
      <div class="card exercise-instance">
        <div class="card-header">
          <div class="row">
            <div class="col-md-6 col-xs-12">
              <h2>
                <xsl:value-of select="position()"/>.
                <xsl:value-of select="./ExerciseName"/>
              </h2>
            </div>
            <div class="col-md-6 col-xs-12 text-md-right meta small">
              <div>
                Total weight lifted:
                <span class="font-weight-bold exercise-set__total-weight"></span>
              </div>
              <div>
                Total sets:
                <span class="font-weight-bold">
                  <xsl:value-of select="count(./ExerciseSets/ExerciseSet)" />
                </span>
              </div>
              <div>
                Estimated one rep max:
                <span class="font-weight-bold">(Todo)</span>
              </div>
            </div>
          </div>
        </div>
        <div class="card-body">
          <div class="table-responsive">
            <table class="table table-striped">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Weight (lbs)</th>
                  <th>Repetitions</th>
                </tr>
              </thead>
              <tbody>
                <xsl:apply-templates select="ExerciseSets" />
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="ExerciseSets">
    <xsl:apply-templates select="ExerciseSet" />
  </xsl:template>
  <xsl:template match="ExerciseSet">
    <tr class="exercise-set">
      <th scope="row">
        <xsl:value-of select="position()" />
      </th>
      <td class="weight">
        <xsl:value-of select="./Weight"/>
      </td>
      <td class="repetitions">
        <xsl:value-of select="./Repetitions"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>